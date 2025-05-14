using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.IO;
using UnityEngine;
using Mono.Data.Sqlite;

public class SqliteDatabase
{
    private const string dbFileName = "CharacterData.db";
 
    private IDbConnection dbConnection;
    private IDbCommand dbCommand;
 
    public void OpenDatabase()
    {
        string path = Path.Join(Application.streamingAssetsPath, dbFileName);

        dbConnection = new SqliteConnection("URI=file:" + path);
        dbConnection.Open();
    }
    

    /// <summary>
    /// 테이블 생성 테스트용 코드
    /// </summary>
    public void DropTable()
    {
        Debug.Log("테이블 삭제");
        //TODO: 테이블 생성 완료되면 삭제할 것
        using (dbCommand = dbConnection.CreateCommand())
        {
            dbCommand.CommandText = @"DROP TABLE IF EXISTS Character";
            dbCommand.ExecuteNonQuery(); 
        }

    }
    
    
    /// <summary>
    /// 캐릭터 데이터 테이블 생성
    /// </summary>
    public void CreateTable()
    {
        //using 문으로 자동 dispose 하도록 설정
        using (dbCommand = dbConnection.CreateCommand())
        { 
            dbCommand.CommandText = @"CREATE TABLE IF NOT EXISTS Character
                                (
                                slot_id        INTEGER PRIMARY KEY NOT NULL,
                                is_create       INTEGER DEFAULT 0,
                                remaining_days  INTEGER NOT NULL DEFAULT 150,
                                strength        INTEGER,
                                agility         INTEGER,
                                intelligence    INTEGER,
                                posX            REAL,
                                posY            REAL
                                )";
            dbCommand.ExecuteNonQuery(); 
        }
    }
    
}