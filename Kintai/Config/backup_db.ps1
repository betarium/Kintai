$BACKUP_DIR = "C:\inetpub\wwwroot\Kintai\Config\backup"

$TARGET_DBSVR = "localhost"

########################################

$now_key = [DateTime]::Now.ToString("yyyyMMdd-HHmmss");

$backup_dir_now = Join-Path $BACKUP_DIR "$now_key"

if (!(Test-Path $backup_dir_now)) {
    mkdir $backup_dir_now
}

sqlcmd -S $TARGET_DBSVR -Q "BACKUP DATABASE Kintai TO DISK='$backup_dir_now\Kintai.bak' WITH INIT"
sqlcmd -S $TARGET_DBSVR -Q "BACKUP DATABASE aspnetdb TO DISK='$backup_dir_now\aspnetdb.bak' WITH INIT"

