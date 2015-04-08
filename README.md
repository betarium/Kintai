# Kintai

##開発環境・動作環境
*Windows XP/8.1
*IIS
*Visual Studio 2010 Express
*SQL Server 2008 Express

##環境構築
*VisualStudioでリビルドする。
*SQLフォルダのSQLファイルでデータベースを作成する。以下の順に実行する。
**aspnetdb.sql
**Kintai.sql
**WorkTime.sql
**HolidayInfo.sql
*Configフォルダの設定ファイルをリネームする。
**AppSettings.Sample.config → AppSettings.config
**ConnectionStrings.Sample.config → ConnectionStrings.config
*設定ファイルのDB接続先とメール設定を修正する。

##ライセンス
Copyright (c) 2015 betarium
The MIT License (MIT)
