using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;

namespace SqlQuery.Editing
{
    public class SqlEditor : RichTextBox
    {
        private readonly Timer highlightTimer;
        private bool suppressCompletion;

        private ToolStripDropDown completionPopup;
        private ListBox completionList;
        private string currentPrefix = string.Empty;

        public SqlCodeCompletion Completion { get; private set; }

        public event EventHandler ExecuteRequested;

        public SqlEditor()
        {
            Completion = new SqlCodeCompletion();
            Font = new Font("Consolas", 10f);
            WordWrap = false;
            AcceptsTab = true;
            HideSelection = false;
            DetectUrls = false;
            highlightTimer = new Timer();
            highlightTimer.Interval = 400;
            highlightTimer.Tick += HighlightTimer_Tick;
        }

        private void HighlightTimer_Tick(object sender, EventArgs e)
        {
            highlightTimer.Stop();
            SqlSyntaxHighlighter.Apply(this);
        }

        protected override void OnTextChanged(EventArgs e)
        {
            base.OnTextChanged(e);
            highlightTimer.Stop();
            highlightTimer.Start();
            if (!suppressCompletion)
            {
                UpdateCompletion();
            }
        }

        protected override void OnKeyDown(KeyEventArgs e)
        {
            base.OnKeyDown(e);
            if (completionPopup != null && completionPopup.Visible)
            {
                if (e.KeyCode == Keys.Down) { MoveSelection(1); e.Handled = true; e.SuppressKeyPress = true; return; }
                if (e.KeyCode == Keys.Up) { MoveSelection(-1); e.Handled = true; e.SuppressKeyPress = true; return; }
                if (e.KeyCode == Keys.PageDown) { MoveSelection(10); e.Handled = true; e.SuppressKeyPress = true; return; }
                if (e.KeyCode == Keys.PageUp) { MoveSelection(-10); e.Handled = true; e.SuppressKeyPress = true; return; }
                if (e.KeyCode == Keys.Enter || e.KeyCode == Keys.Tab) { AcceptCompletion(); e.Handled = true; e.SuppressKeyPress = true; return; }
                if (e.KeyCode == Keys.Escape) { HideCompletion(); e.Handled = true; e.SuppressKeyPress = true; return; }
            }
            if (e.Control && e.KeyCode == Keys.Space)
            {
                ShowCompletions(Completion.GetAll(60), GetWordBeforeCaret());
                e.Handled = true;
                e.SuppressKeyPress = true;
                return;
            }
            if (e.KeyCode == Keys.F5 && ExecuteRequested != null)
            {
                ExecuteRequested(this, EventArgs.Empty);
                e.Handled = true;
                e.SuppressKeyPress = true;
            }
        }

        protected override void OnLostFocus(EventArgs e)
        {
            base.OnLostFocus(e);
            if (completionPopup == null || !completionPopup.Visible)
            {
                return;
            }
            if (completionPopup.ContainsFocus)
            {
                return;
            }
            if (completionPopup.RectangleToScreen(completionPopup.ClientRectangle).Contains(Cursor.Position))
            {
                return;
            }
            HideCompletion();
        }

        protected override void OnVScroll(EventArgs e)
        {
            base.OnVScroll(e);
            HideCompletion();
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                highlightTimer.Dispose();
                if (completionPopup != null)
                {
                    completionPopup.Dispose();
                }
            }
            base.Dispose(disposing);
        }

        private void MoveSelection(int delta)
        {
            int index = completionList.SelectedIndex + delta;
            if (index < 0)
            {
                index = 0;
            }
            if (index >= completionList.Items.Count)
            {
                index = completionList.Items.Count - 1;
            }
            completionList.SelectedIndex = index;
        }

        private string GetWordBeforeCaret()
        {
            int end = SelectionStart;
            int start = end;
            while (start > 0)
            {
                char c = Text[start - 1];
                if (char.IsLetterOrDigit(c) || c == '_')
                {
                    start--;
                }
                else
                {
                    break;
                }
            }
            return Text.Substring(start, end - start);
        }

        private void UpdateCompletion()
        {
            string prefix = GetWordBeforeCaret();
            if (prefix.Length == 0 || char.IsDigit(prefix[0]))
            {
                HideCompletion();
                return;
            }
            List<string> items = Completion.GetCompletions(prefix, 40);
            if (items.Count == 0)
            {
                HideCompletion();
                return;
            }
            ShowCompletions(items, prefix);
        }

        private void ShowCompletions(List<string> items, string prefix)
        {
            currentPrefix = prefix;
            EnsurePopup();
            completionList.BeginUpdate();
            completionList.Items.Clear();
            foreach (string item in items)
            {
                completionList.Items.Add(item);
            }
            completionList.EndUpdate();
            if (completionList.Items.Count == 0)
            {
                HideCompletion();
                return;
            }
            completionList.SelectedIndex = 0;
            int height = Math.Min(completionList.Items.Count, 12) * completionList.ItemHeight + 4;
            if (height < 40)
            {
                height = 40;
            }
            ToolStripControlHost host = (ToolStripControlHost)completionPopup.Items[0];
            host.Size = new Size(360, height);
            completionList.Size = host.Size;
            Point location = GetPositionFromCharIndex(SelectionStart);
            location.Y += Font.Height + 2;
            location.X += 2;
            if (completionPopup.Visible)
            {
                completionPopup.Location = PointToScreen(location);
            }
            else
            {
                completionPopup.Show(this, location);
            }
            Focus();
        }

        private void EnsurePopup()
        {
            if (completionPopup != null)
            {
                return;
            }
            completionList = new ListBox();
            completionList.BorderStyle = BorderStyle.None;
            completionList.IntegralHeight = false;
            completionList.Font = new Font("Consolas", 9f);
            completionList.Click += delegate { AcceptCompletion(); };
            completionList.DoubleClick += delegate { AcceptCompletion(); };
            ToolStripControlHost host = new ToolStripControlHost(completionList);
            host.Margin = Padding.Empty;
            host.Padding = Padding.Empty;
            host.AutoSize = false;
            completionPopup = new ToolStripDropDown();
            completionPopup.AutoClose = false;
            completionPopup.Margin = Padding.Empty;
            completionPopup.Padding = Padding.Empty;
            completionPopup.DropShadowEnabled = true;
            completionPopup.Items.Add(host);
        }

        private void AcceptCompletion()
        {
            if (completionList == null || completionList.SelectedItem == null)
            {
                HideCompletion();
                return;
            }
            string text = Convert.ToString(completionList.SelectedItem);
            HideCompletion();
            suppressCompletion = true;
            try
            {
                int end = SelectionStart;
                int start = end - currentPrefix.Length;
                if (start < 0)
                {
                    start = 0;
                }
                SelectionStart = start;
                SelectionLength = end - start;
                SelectedText = text;
            }
            finally
            {
                suppressCompletion = false;
            }
            Focus();
        }

        private void HideCompletion()
        {
            if (completionPopup != null && completionPopup.Visible)
            {
                completionPopup.Close();
            }
        }
    }
}
