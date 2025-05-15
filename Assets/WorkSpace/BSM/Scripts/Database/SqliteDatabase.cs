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

    private IDataReader dbDataReader;

    private int slotCount = 3;

    public SqliteDatabase()
    {
        string path = Path.Join(Application.streamingAssetsPath, dbFileName);

        dbConnection = new SqliteConnection("URI=file:" + path);
        dbConnection.Open();
    }

    ~SqliteDatabase()
    {
        dbConnection.Close();
        dbConnection = null;
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
            dbCommand.CommandText = @"
                                CREATE TABLE IF NOT EXISTS Character
                                (
                                slot_id             INTEGER PRIMARY KEY NOT NULL,
                                is_create           INTEGER DEFAULT 0,
                                hair_sprite         TEXT NOT NULL DEFAULT 'none',
                                body_sprite         TEXT NOT NULL DEFAULT 'none',
                                shirt_sprite        TEXT NOT NULL DEFAULT 'none', 
                                weapon_sprite       TEXT NOT NULL DEFAULT 'none',
                                last_played_time    TEXT NOT NULL DEFAULT 'none',
                                remaining_days      INTEGER NOT NULL DEFAULT 150,
                                strength            INTEGER NOT NULL DEFAULT 0,
                                agility             INTEGER NOT NULL DEFAULT 0,
                                intelligence        INTEGER NOT NULL DEFAULT 0,
                                objective_item      INTEGER NOT NULL DEFAULT 'none'
                                )";

            dbCommand.ExecuteNonQuery();
        }

        //테이블 초기 컬럼값 삽입
        using (dbCommand = dbConnection.CreateCommand())
        {
            dbCommand.CommandText = "SELECT COUNT(*) FROM Character";
            long count = (long)dbCommand.ExecuteScalar();

            if (count != 0) return;

            for (int i = 0; i < slotCount; i++)
            {
                using (dbCommand = dbConnection.CreateCommand())
                {
                    dbCommand.CommandText = @"INSERT INTO Character (slot_id)
                                        VALUES (@slot_id)";
                    dbCommand.Parameters.Clear();
                    dbCommand.Parameters.Add(new SqliteParameter("@slot_id", i + 1));
                    dbCommand.ExecuteNonQuery();
                }
            }
        }
    }

    /// <summary>
    /// 게임 저장 시 변경 내용 DB 업데이트
    /// </summary>
    public void UpdateTable()
    {
    }

    /// <summary>
    /// DB 컬럼 가져오기
    /// </summary>
    /// <param name="column">가져올 컬럼명</param>
    /// <param name="where">wher절 조건</param>
    /// <param name="whereValue">조건의 값</param>
    /// <typeparam name="T">여러 타입으로 조회</typeparam>
    /// <returns></returns>
    public IDataReader ReadTable<T>(string column, string where = "", T whereValue = default(T))
    {
        using (dbCommand = dbConnection.CreateCommand())
        {
            string query = $"SELECT {column} FROM Character ";

            if (!string.IsNullOrEmpty(where))
            {
                query += $"WHERE {where} = @value";
                
                dbCommand.Parameters.Clear();
                dbCommand.Parameters.Add(new SqliteParameter("@value", whereValue));
            }

            dbCommand.CommandText = query; 
            dbDataReader = dbCommand.ExecuteReader();
        }

        return dbDataReader;
    }
}