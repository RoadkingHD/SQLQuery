# SqlQuery

A full-English Windows Forms (C#, .NET Framework 4.7.2) desktop tool to connect to and query **Microsoft SQL Server**, **SQLite**, **PostgreSQL**, and **MySQL** databases from a single UI.

## Features

- **Multi-provider connections**
  - SQL Server (System.Data.SqlClient, Windows or SQL authentication, optional encryption)
  - SQLite (System.Data.SQLite, file based)
  - PostgreSQL (Npgsql)
  - MySQL (MySqlConnector)
- **New Connection dialog**: pick the server type, enter server/credentials, click **Test Connection** — on success the **Database** combo box is filled with the databases available on the server (SQLite asks for a file instead). Clicking **Connect** (OK) connects immediately. **Connection strings are not saved**; nothing is persisted between sessions.
- **Schema browser (TreeView)** with icons: **tables, views, and stored procedures** grouped per provider, with columns shown on the Columns tab when a table or view is selected.
- **Tables tab**: all tables and views with schema and type.
- **Columns tab**: columns of the table selected in the tree (name, type, nullability, default, identity).
- **Rights tab**: **effective** permissions of the connected user
  - SQL Server: `HAS_PERMS_BY_NAME` over SELECT / INSERT / UPDATE / DELETE / ALTER / CONTROL for every table and view, plus column-level grants
  - PostgreSQL: `has_table_privilege` over SELECT / INSERT / UPDATE / DELETE / REFERENCES / TRIGGER, plus column-level privileges
  - MySQL: `information_schema` table and column privileges
  - SQLite: informational note (SQLite has no per-user privileges; access control is file-level)
- **Schema DDL tab**: generated `CREATE TABLE` script for the whole database (SQLite shows its native DDL from `sqlite_master`).
- **Query tab with SQL editor**:
  - **Syntax coloring** (keywords blue, comments green, strings red, numbers teal) with flicker-free repaint
  - **Code completion** (Ctrl+Space or as you type): SQL keywords, functions, tables, and columns of the connected database; popup navigable with arrows, Enter/Tab to accept, Esc to close, and it stays open while you type
  - Execute with **F5** or the toolbar; multiple result sets supported; results grid plus messages pane with row counts and elapsed time
- **Right-click a table or view in the tree → "Select Top 1000 Rows"** to query any table instantly.
- **Right-click a table or view → "Dump as C# Class"**: the generated POCO class is shown in the **"C# code" tab**, with a **Copy to Clipboard** button (per-provider CLR type mapping, `Nullable<T>` for nullable value-type columns, PascalCase property names).

## Project layout

```
SqlQuery.sln
SqlQuery/
  Program.cs
  App.config
  Properties/AssemblyInfo.cs
  Db/
    ConnectionInfo.cs        connection model + connection-string builders
    DbProviderBase.cs        abstract provider + shared helpers (query, DDL + C# class generation)
    SqlServerProvider.cs     SQL Server metadata / rights / DDL
    SqliteProvider.cs        SQLite metadata / DDL
    PostgresProvider.cs      PostgreSQL metadata / rights / DDL
    MySqlProvider.cs         MySQL metadata / rights / DDL
    TableRef.cs              schema-qualified table reference
  Editing/
    SqlSyntaxHighlighter.cs  tokenizer + keyword set + coloring
    SqlCodeCompletion.cs     completion source (keywords, functions, schema objects)
    SqlEditor.cs             RichTextBox-based editor with highlighting + completion popup
  Forms/
    MainForm.cs / .Designer.cs / .resx
    ConnectionDialog.cs / .Designer.cs / .resx
```

Each WinForms source is split into `*.cs` (logic) and `*.Designer.cs` (layout), as required.

## Building

Requirements: Visual Studio 2019/2022 with the ".NET desktop development" workload (or MSBuild + NuGet).

1. Open `SqlQuery.sln` in Visual Studio and press **F5**; or from a Developer Command Prompt:

   ```
   nuget restore SqlQuery.sln
   msbuild SqlQuery.sln /t:Restore;Build /p:Configuration=Release
   ```

   (The project uses `PackageReference`, so `msbuild /t:Restore` alone is enough with VS 2017+.)

NuGet packages used:

- `Npgsql` 4.1.14 (PostgreSQL, supports .NET Framework 4.6.1+)
- `System.Data.SQLite.Core` 1.0.118 (SQLite with native interop)
- `MySqlConnector` 2.3.7 (MySQL, .NET Framework 4.6.1+)

SQL Server support is built into .NET Framework (System.Data.SqlClient) — no package needed.

## Notes

- Connections are **not persisted**. Every session starts unconnected; use **New Connection...** and connect in the dialog.
- The "Rights" tab lists the **effective** permissions of the connected user, so owners/admins see their implicit rights too (explicit grants only are often empty on development databases).
- The Schema DDL for SQL Server/PostgreSQL/MySQL is generated from catalog metadata (columns, nullability, defaults, primary key). Other constraints/indexes are listed in the Rights tab but not scripted.
