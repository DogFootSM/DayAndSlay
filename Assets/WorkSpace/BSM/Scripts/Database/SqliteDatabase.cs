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
    private string path = "";
    
    public SqliteDatabase()
    {
        path = Path.Join(Application.streamingAssetsPath, dbFileName);
        ConnectOpen();
    }

    ~SqliteDatabase()
    {
        ConnectDispose();
    }

    public void ConnectOpen()
    {
        if (dbConnection == null)
        {
            dbConnection = new SqliteConnection("URI=file:" + path);
        }
        
        if (dbConnection.State == ConnectionState.Closed)
        {
            dbConnection.Open();
            
            //Item, Skill Table 외래키 연결 재설정
            using (dbCommand = dbConnection.CreateCommand())
            {
                dbCommand.CommandText = "PRAGMA foreign_keys = ON";
                dbCommand.ExecuteNonQuery();
            }
        } 
    }
    
    public void ConnectDispose()
    {
        dbConnection.Dispose();
        dbConnection.Close();
        dbConnection = null;
    }
    
    /// <summary>
    /// 테이블 생성 테스트용 코드
    /// </summary>
    public void DropTable()
    {
        Debug.Log("테이블 삭제");
        using (dbCommand = dbConnection.CreateCommand())
        {
            dbCommand.CommandText = @"DROP TABLE IF EXISTS Character";
            dbCommand.ExecuteNonQuery();
            
            dbCommand.CommandText = @"DROP TABLE IF EXISTS CharacterItem";
            dbCommand.ExecuteNonQuery();
            
            dbCommand.CommandText = @"DROP TABLE IF EXISTS CharacterSkill";
            dbCommand.ExecuteNonQuery();
        }
    }
    
    /// <summary>
    /// 캐릭터 데이터 테이블 생성
    /// </summary>
    public void CreateTable()
    {
        //using 문으로 자동 dispose 하도록 설정
        //캐릭터 테이블 생성
        using (dbCommand = dbConnection.CreateCommand())
        {
            //TODO: 모자, 망토 추가
            dbCommand.CommandText = @"
                                CREATE TABLE IF NOT EXISTS Character
                                (
                                slot_id             INTEGER PRIMARY KEY NOT NULL,
                                is_create           INTEGER DEFAULT 0,
                                hair_sprite         TEXT NOT NULL DEFAULT 'none',
                                body_sprite         TEXT NOT NULL DEFAULT 'none',
                                shirt_sprite        TEXT NOT NULL DEFAULT 'none', 
                                weapon_sprite       TEXT NOT NULL DEFAULT 'none',
                                weapon_tier         INTEGER NOT NULL DEFAULT 0,
                                gold                INTEGER NOT NULL DEFAULT 0,
                                weapon_type         INTEGER NOT NULL DEFAULT 0,
                                currentday          INTEGER NOT NULL DEFAULT 1,
                                exp                 INTEGER NOT NULL DEFAULT 0,
                                stats_point         INTEGER NOT NULL DEFAULT 0,
                                skill_point         INTEGER NOT NULL DEFAULT 0,
                                char_level          INTEGER NOT NULL DEFAULT 1,
                                strength            INTEGER NOT NULL DEFAULT 5,
                                agility             INTEGER NOT NULL DEFAULT 4,
                                intelligence        INTEGER NOT NULL DEFAULT 3,
                                debt                INTEGER NOT NULL DEFAULT 1000000,
                                hasdungeonentered   INTEGER DEFAULT 0,
                                is1stagecleared     INTEGER DEFAULT 0,
                                is2stagecleared     INTEGER DEFAULT 0,
                                is3stagecleared     INTEGER DEFAULT 0,
                                currenttime         INTEGER DEFAULT 1
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
                                    item_id             INTEGER NOT NULL,
                                    slot_id             INTEGER NOT NULL,
                                    item_amount         INTEGER NOT NULL,
                                    inventory_slot_id   INTEGER NOT NULL,
                                    is_equipment        INTEGER NOT NULL DEFAULT 0,
                                    PRIMARY KEY(slot_id, item_id)
                                    FOREIGN KEY (slot_id) REFERENCES Character (slot_id) ON DELETE CASCADE
                                )";
            dbCommand.ExecuteNonQuery();
        }

        //캐릭터 스킬 테이블 생성
        using (dbCommand = dbConnection.CreateCommand())
        {
            dbCommand.CommandText = "PRAGMA foreign_keys = ON";
            dbCommand.ExecuteNonQuery();
            
            dbCommand.CommandText = @"
                                    CREATE TABLE IF NOT EXISTS CharacterSkill
                                    (
                                        slot_id         INTEGER NOT NULL,
                                        skill_id        TEXT NOT NULL,
                                        skill_level     INTEGER NOT NULL,
                                        skill_unlocked  INTEGER NOT NULL, 
                                        PRIMARY KEY (slot_id, skill_id)
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
    public bool CharacterUpdateTable(string[] column, string[] columnValue, string condition, string conditionValue)
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
            int result = dbCommand.ExecuteNonQuery();

            return result > 0;
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
    public bool ItemUpsertTable(string[] column, string[] columnValue)
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
            
            for (int i = 0; i < columnValue.Length; i++)
            {
                query += $"@value{i}";
                
                dbCommand.Parameters.Add(new SqliteParameter($"@value{i}", columnValue[i]));

                if (i < columnValue.Length - 1)
                {
                    query += ", ";
                } 
            }
            
            query += ") ON CONFLICT(slot_id, item_id) DO UPDATE SET item_amount = excluded.item_amount, inventory_slot_id = excluded.inventory_slot_id, is_equipment = excluded.is_equipment";
            dbCommand.CommandText = query;
            int result = dbCommand.ExecuteNonQuery();

            return result > 0;
        }
    }

    /// <summary>
    /// 아이템 데이터 테이블 조회
    /// </summary>
    /// <param name="condition">조회할 조건</param>
    /// <param name="conditionValue">조회할 조건의 값</param>
    /// <returns></returns>
    /// <exception cref="ArgumentException"></exception>
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
    
    /// <summary>
    /// 아이템 삭제 테이블
    /// </summary>
    public void ItemDeleteTable()
    {
        
    }

    /// <summary>
    /// 초기 캐릭터 생성 시 스킬 데이터 테이블 삽입
    /// </summary>
    /// <param name="slotID">현재 생성한 슬롯 ID</param> 
    public void SkillInsertTable(string slotID)
    {  
        using (dbCommand = dbConnection.CreateCommand())
        {
            //SQL, 즉 c에서는 \ 이스케이프 문자로 쓰여 경로를 제대로 인식하지 못함
            string path = Path.Combine(Application.streamingAssetsPath, "SkillConfig.db").Replace("\\", "/");
            
            dbCommand.CommandText = $"ATTACH DATABASE '{path}' AS SkillConfig";
            dbCommand.ExecuteNonQuery();

            string query = "INSERT INTO CharacterSkill (slot_id, skill_id, skill_level, skill_unlocked) " +
                           "SELECT @slot, skill_id, skill_level, skill_unlocked " +
                           "FROM SkillConfig.Skills";

            dbCommand.Parameters.Add(new SqliteParameter("@slot", slotID));
            
            dbCommand.CommandText = query;
            dbCommand.ExecuteNonQuery();
        } 
    }

    /// <summary>
    /// 스킬 테이블 업데이트
    /// </summary>
    /// <param name="column">스킬 테이블 컬럼명</param>
    /// <param name="columnValue">업데이트할 행 데이터 값</param>
    /// <param name="condition">업데이트 조건</param>
    /// <param name="conditionValue">조건의 값</param>
    public bool SkillUpdateTable(string[] column, string[] columnValue, string[] condition, string[] conditionValue)
    {
        using (dbCommand = dbConnection.CreateCommand())
        {
            string query = "UPDATE CharacterSkill SET ";

            for (int i = 0; i < column.Length; i++)
            {
                query += $"{column[i]} = @set{i}";
                dbCommand.Parameters.Add(new SqliteParameter($"@set{i}", columnValue[i]));
                 
                if (i < column.Length - 1)
                {
                    query += ", ";
                } 
            }
             
            query += " WHERE ";
            
            for (int con = 0; con < condition.Length; con++)
            {
                query += $"{condition[con]} = @con{con}";
                dbCommand.Parameters.Add(new SqliteParameter($"@con{con}", conditionValue[con]));
         
                if (con < condition.Length - 1)
                {
                    query += " AND ";
                } 
            }  
             
            dbCommand.CommandText = query;
            int result = dbCommand.ExecuteNonQuery();

            return result > 0;
        } 
    }
    
    /// <summary>
    /// 현재 슬롯의 캐릭터 스킬 데이터를 읽어옴
    /// </summary>
    /// <param name="slotID">현재 캐릭터 슬롯 ID</param>
    public IDataReader SkillReadTable(string slotID)
    {
        using (dbCommand = dbConnection.CreateCommand())
        {
            string query = $"SELECT skill_id, skill_level, skill_unlocked FROM CharacterSkill WHERE slot_id = @slot";
            dbCommand.Parameters.Add(new SqliteParameter("@slot", slotID));
            dbCommand.CommandText = query;

            dbDataReader = dbCommand.ExecuteReader(); 
        } 
        
        return dbDataReader;
    }
    
}