using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Text;
using UnityEngine;
using Mono.Data.Sqlite;
using Zenject;

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
                                weapon_type         INTEGER NOT NULL DEFAULT 0,
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
    /// 컬럼 DB 업데이트
    /// </summary>
    public void UpdateTable(string[] column, string[] columnValue, string condition, string conditionValue)
    {
        condition ??= string.Empty;
        conditionValue ??= string.Empty;
        
        if (column.Length == 0 || columnValue.Length == 0 || (column.Length != columnValue.Length))
        {
            throw new ArgumentException("Update 조건 오류");
        }

        string query = "UPDATE Character SET ";

        using (dbCommand = dbConnection.CreateCommand())
        {
            for (int i = 0; i < column.Length; i++)
            { 
                query += $"{column[i]} = @columnValue{i}"; 
  
                dbCommand.Parameters.Add(new SqliteParameter($"@columnValue{i}", columnValue[i]));
                
                if (i < column.Length - 1)
                {
                    query += ", ";
                }
            }

            if (!string.IsNullOrEmpty(condition))
            {
                query += $" WHERE {condition} = @value";
                dbCommand.Parameters.Add(new SqliteParameter("@value", conditionValue));
            }
 
            dbCommand.CommandText = query;
            dbCommand.ExecuteNonQuery();
        }
    }
 
    /// <summary>
    /// 단일, 다중 컬럼 조회
    /// </summary>
    /// <param name="column">가져올 컬럼명</param>
    /// <param name="condition">where절 조건</param>
    /// <param name="conditionValue">조건의 값</param>
    /// <param name="operation">조건 연산 기호</param>
    /// <typeparam name="T">여러 타입으로 조회</typeparam>
    /// <returns></returns>
    public IDataReader ReadTable(string[] column, string[] condition, string[] conditionValue, string[] operation)
    {
        condition ??= Array.Empty<string>();
        conditionValue ??= Array.Empty<string>();
        operation ??= Array.Empty<string>();

        if (condition.Length > 0
            && (condition.Length != conditionValue.Length || condition.Length - 1 != operation.Length))
        {
            throw new ArgumentException("DB 조회 조건이 잘못 됐음");
        }

        using (dbCommand = dbConnection.CreateCommand())
        {
            string query = "SELECT ";

            //컬럼의 개수만큼 추가
            for (int i = 0; i < column.Length; i++)
            {
                query += $"{column[i]}";

                //뒤에 컬럼이 존재한다면 , 추가
                if (i < column.Length - 1)
                {
                    query += ",";
                }
            }

            query += " FROM Character";

            //조건이 있을 경우에만 WHERE절 추가
            if (condition.Length > 0)
            {
                query += " WHERE ";
            }

            for (int j = 0; j < condition.Length; j++)
            {
                //WHERE절 조건 추가
                query += $"{condition[j]} = " + $"@value{j}";

                if (j < condition.Length - 1 && j < operation.Length)
                {
                    query += $" {operation[j]} ";
                }

                dbCommand.Parameters.Add(new SqliteParameter($"@value{j}", conditionValue[j]));
            }

            dbCommand.CommandText = query;
            dbDataReader = dbCommand.ExecuteReader();
        }

        return dbDataReader;
    }
}