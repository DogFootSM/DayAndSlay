using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using UnityEngine;
using Zenject;


public class SqlManager : IInitializable
{
    private SqliteDatabase sqlDatabase;

    private Dictionary<CharacterDataColumns, string> CharacterDataColumns;
    
    /// <summary>
    /// 게임 시작 시 테이블 생성
    /// </summary>
    public void Initialize()
    {
        CharacterDataColumns = new Dictionary<CharacterDataColumns, string>();
        sqlDatabase = new SqliteDatabase();
        sqlDatabase.CreateTable(); 
    }

    /// <summary>
    /// 캐릭터 데이터 컬럼 반환
    /// </summary>
    /// <param name="columns">컬럼 키</param>
    /// <returns></returns>
    public string CharacterColumn(CharacterDataColumns columns)
    {
        if (!CharacterDataColumns.ContainsKey(columns))
        {
            CharacterDataColumns.TryAdd(columns, columns.ToString().ToLower());
        }   
        
        return CharacterDataColumns[columns];
    }
    
    /// <summary>
    /// 캐릭터 데이터 컬럼 추가
    /// </summary>
    /// <param name="column">추가할 컬럼명</param>
    /// <param name="columnValue">추가할 컬럼 값</param>
    public void CharacterInsertTable(string[] column, string[] columnValue)
    {
        sqlDatabase.CharacterInsertTable(column, columnValue);
    }
    
    
    /// <summary>
    /// 조건 컬럼 데이터 반환
    /// </summary>
    /// <param name="columnName"></param>
    /// <param name="where"></param>
    /// <param name="whereValue"></param>
    /// <typeparam name="T"></typeparam>
    /// <returns></returns>
    public IDataReader ReadDataColumn(string[] columnName, string[] where, string[] whereValue, string[] operation)
    {
        return sqlDatabase.CharacterReadTable(columnName, where, whereValue, operation);
    }

    /// <summary>
    /// 컬럼 업데이트
    /// </summary>
    /// <param name="columnName">업데이트 할 컬럼</param>
    /// <param name="columnValue">업데이트 값</param>
    /// <param name="condition">업데이트 조건</param>
    /// <param name="conditionValue">업데이트 조건 값</param>
    public void UpdateDataColumn(string[] columnName, string[] columnValue, string condition, string conditionValue)
    {
        sqlDatabase.CharacterUpdateTable(columnName, columnValue, condition, conditionValue);
    }

    /// <summary>
    /// 캐릭터 DB 행 삭제
    /// </summary>
    /// <param name="condition">삭제할 행 조건</param>
    /// <param name="conditionValue">삭제할 행 조건의 값</param>
    public void DeleteDataColumn(string condition, string conditionValue)
    {
        sqlDatabase.CharacterDeleteTable(condition, conditionValue);
    }
    
    /// <summary>
    /// 아이템 컬럼 삽입 or 업데이트
    /// </summary>
    /// <param name="columnName">변경할 컬러명</param>
    /// <param name="columnValue">변경할 컬럼값</param>
    public void UpsertItemDataColumn(string[] columnName, string[] columnValue)
    {
        sqlDatabase.ItemUpsertTable(columnName, columnValue);
    }
    
}