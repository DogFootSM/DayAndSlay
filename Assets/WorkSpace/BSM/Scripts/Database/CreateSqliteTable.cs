using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.IO;
using UnityEngine;
using Mono.Data.Sqlite;

public class CreateSqliteTable : MonoBehaviour
{
    private const string dbFileName = "CharacterData.db";


    private IDbConnection dbConnection;
    private IDbCommand dbCommand;
    
    private void Start()
    {
        string path = Path.Join(Application.streamingAssetsPath, dbFileName);

        dbConnection = new SqliteConnection("URI=file:" + path);
        dbConnection.Open();

        DropTable();
        CreateTable();
        
        dbConnection.Close(); 
    }


    /// <summary>
    /// 테이블 생성 테스트용 코드
    /// </summary>
    public void DropTable()
    {
        //TODO: 테이블 생성 완료되면 삭제할 것
        using (dbCommand = dbConnection.CreateCommand())
        {
            dbCommand.CommandText = @"DROP TABLE IF EXISTS Character";
            dbCommand.ExecuteNonQuery(); 
        }

    }
    
    
    public void CreateTable()
    {
        using (dbCommand = dbConnection.CreateCommand())
        { 
            dbCommand.CommandText = @"CREATE TABLE IF NOT EXISTS Character
                                (slot_id INTEGER PRIMARY KEY NOT NULL,
                                is_create INTEGER DEFAULT 0)";
            dbCommand.ExecuteNonQuery(); 
        }
    }
    
}