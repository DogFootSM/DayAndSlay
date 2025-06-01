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
        
        //보유 아이템 데이터 테이블 생성
        using (dbCommand = dbConnection.CreateCommand())
        {
            //Sqlite는 기본적으로 외래키 설정이 Off로 되어 있음
            dbCommand.CommandText = "PRAGMA foreign_keys = ON";
            dbCommand.ExecuteNonQuery();
            
            dbCommand.CommandText = @"

                                CREATE TABLE IF NOT EXISTS CharacterItem
                                (
                                    item_id         INTEGER NOT NULL,
                                    slot_id         INTEGER NOT NULL,
                                    item_amount      INTEGER NOT NULL,
                                    inventory_slot_id INTEGER NOT NULL,
                                    PRIMARY KEY(slot_id, item_id)
                                    FOREIGN KEY (slot_id) REFERENCES Character (slot_id) ON DELETE CASCADE
                                )";
            dbCommand.ExecuteNonQuery();
        }
    }
    
    
    /// <summary>
    /// 캐릭터 테이블 데이터 삽입
    /// </summary>
    /// <param name="column">삽입할 컬러명</param>
    /// <param name="columnValue">삽입할 컬럼 값</param>
    public void CharacterInsertTable(string[] column, string[] columnValue)
    {
        column ??= Array.Empty<string>();
        columnValue ??= Array.Empty<string>();
        
        using (dbCommand = dbConnection.CreateCommand())
        {
            string query = "INSERT INTO Character (";

            for (int i = 0; i < columnValue.Length; i++)
            {
                query += column[i];

                if (i < columnValue.Length - 1)
                {
                    query += ", ";
                } 
            }

            query += ") VALUES (";

            for (int i = 0; i < columnValue.Length; i++)
            {
                query += $"@slot_id{i}";
                dbCommand.Parameters.Clear();
                dbCommand.Parameters.Add(new SqliteParameter($"@slot_id{i}", columnValue[i]));

                if (i < columnValue.Length - 1)
                {
                    query += ", ";
                } 
            }
            
            query += ")";
            
            dbCommand.CommandText = query;
            dbCommand.ExecuteNonQuery(); 
        }
        
    }
    
    /// <summary>
    /// 캐릭터 컬럼 DB 업데이트
    /// </summary>
    public void CharacterUpdateTable(string[] column, string[] columnValue, string condition, string conditionValue)
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
    /// 캐릭터 테이블 단일, 다중 컬럼 조회
    /// </summary>
    /// <param name="column">가져올 컬럼명</param>
    /// <param name="condition">where절 조건</param>
    /// <param name="conditionValue">조건의 값</param>
    /// <param name="operation">조건 연산 기호</param>
    /// <returns></returns>
    public IDataReader CharacterReadTable(string[] column, string[] condition, string[] conditionValue, string[] operation)
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

    /// <summary>
    /// 캐릭터 DB 행 삭제
    /// </summary>
    /// <param name="condition">삭제할 행 조건</param>
    /// <param name="conditionValue">삭제할 조건의 값</param>
    public void CharacterDeleteTable(string condition, string conditionValue)
    { 
        condition ??= string.Empty;
        conditionValue ??= string.Empty;
        
        using (dbCommand = dbConnection.CreateCommand())
        {
            string query = $"DELETE FROM Character WHERE {condition} = @value";
                
            dbCommand.Parameters.Add(new SqliteParameter($"@value", conditionValue));
            
            dbCommand.CommandText = query;
            dbCommand.ExecuteNonQuery(); 
        } 
    }
    
    /// <summary>
    /// 아이템 데이터 Update Or Insert
    /// </summary>
    /// <param name="column">변경할 컬럼명</param>
    /// <param name="columnValue">변경할 컬럼 값</param>
    public void ItemUpsertTable(string[] column, string[] columnValue)
    {
        column ??= Array.Empty<string>();
        columnValue ??= Array.Empty<string>();
        
        using (dbCommand = dbConnection.CreateCommand())
        {
            string query = "INSERT INTO CharacterItem (";

            for (int i = 0; i < column.Length; i++)
            {
                query += $"{column[i]}";

                if (i < column.Length - 1)
                {
                    query += ", ";
                } 
            }

            query += ") VALUES (";
            
            Debug.Log(columnValue.Length);
            
            for (int i = 0; i < columnValue.Length; i++)
            {
                query += $"@value{i}";
                
                dbCommand.Parameters.Add(new SqliteParameter($"@value{i}", columnValue[i]));

                if (i < columnValue.Length - 1)
                {
                    query += ", ";
                } 
            }
            
            query += ") ON CONFLICT(slot_id, item_id) DO UPDATE SET item_amount = excluded.item_amount";
            
            Debug.Log(query);
            
            dbCommand.CommandText = query;
            dbCommand.ExecuteNonQuery();
        }
    }

    public IDataReader ItemReadTable(string condition, string conditionValue)
    {
        if (condition == null || conditionValue == null)
        {
            throw new ArgumentException("아이템 데이터 읽어오기 실패"); 
        }
         
        using (dbCommand = dbConnection.CreateCommand())
        {
            string query = "SELECT * FROM CharacterItem WHERE ";

            query += $"{condition} = @value";

            dbCommand.Parameters.Add(new SqliteParameter("@value", conditionValue));
            
            dbCommand.CommandText = query;
            dbDataReader = dbCommand.ExecuteReader();
        }

        return dbDataReader;
    }
    
    
    public void ItemDeleteTable()
    {
        
    }
    
}