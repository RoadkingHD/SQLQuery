# SqlQuery

A full-English Windows Forms (C#, .NET Framework 4.7.2) desktop tool to connect to and query **Microsoft SQL Server**, **SQLite**, and **PostgreSQL** databases from a single UI.

## Features

- **Multi-provider connections**
  - SQL Server (System.Data.SqlClient, Windows or SQL authentication, optional encryption)
  - SQLite (System.Data.SQLite, file based)
  - PostgreSQL (Npgsql)
- **Connection string builder dialog** (`Connection Properties`) with live connection-string preview, **Test Connection**, and **Save** of named connections (persisted to `%APPDATA%\SqlQuery\connections.json`).
- **Schema browser (TreeView)**: tables and views per schema, lazily expanded Columns / Primary Key / Indexes nodes.
- **Tables tab**: all tables and views with schema and type.
- **Columns tab**: columns of the table selected in the tree (name, type, nullability, default, identity).
- **Rights tab**: all table-level and column-level (field) permissions
  - SQL Server: `sys.database_permissions` (object-level and column-level grants)
  - PostgreSQL: `aclexplode` over `pg_class.relacl` / `pg_attribute.attacl` (includes PUBLIC grants)
  - SQLite: informational note (SQLite has no per-user privileges; access control is file-level)
- **Schema DDL tab**: generated `CREATE TABLE` script for the whole database (SQLite shows its native DDL from `sqlite_master`).
- **Query tab with SQL editor**:
  - **Syntax coloring** (keywords blue, comments green, strings red, numbers teal) with flicker-free repaint
  - **Code completion** (Ctrl+Space or as you type): SQL keywords, functions, tables, and columns of the selected table; popup navigable with arrows, Enter/Tab to accept, Esc to close
  - Execute with **F5** or the toolbar; multiple result sets supported; results grid plus messages pane with row counts and elapsed time
- **Right-click a table in the tree → "Select Top 1000 Rows"** to query any table instantly.

## Project layout

```
SqlQuery.sln
SqlQuery/
  Program.cs
  App.config
  Properties/AssemblyInfo.cs
  Db/
    ConnectionInfo.cs        connection model + connection-string builders
    ConnectionStore.cs       save/load named connections (JSON)
    DbProviderBase.cs        abstract provider + shared helpers (query, DDL generation)
    SqlServerProvider.cs     SQL Server metadata / rights / DDL
    SqliteProvider.cs        SQLite metadata / DDL
    PostgresProvider.cs      PostgreSQL metadata / rights / DDL
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

SQL Server support is built into .NET Framework (System.Data.SqlClient) — no package needed.

## Notes

- Saved connections, including passwords, are stored unencrypted in `%APPDATA%\SqlQuery\connections.json`.
- The "Rights" tab shows permissions visible to the connected user (database-level grants for SQL Server, ACLs of relations/attributes for PostgreSQL).
- The Schema DDL for SQL Server/PostgreSQL is generated from catalog metadata (columns, nullability, defaults, primary key). Other constraints/indexes are listed in the tree and Rights tabs but not scripted.
