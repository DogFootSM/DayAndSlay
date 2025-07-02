using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using UnityEngine;
using Zenject;


public class SqlManager : IInitializable
{
    private SqliteDatabase sqlDatabase;

    private Dictionary<CharacterDataColumns, string> _characterDataColumns;
    private Dictionary<CharacterItemDataColumns, string> _characterItemDataColumns;
    private Dictionary<CharacterSkillDataColumns, string> _characterSkillDataColumns;
    
    
    /// <summary>
    /// 게임 시작 시 테이블 생성
    /// </summary>
    public void Initialize()
    {
        _characterDataColumns = new Dictionary<CharacterDataColumns, string>();
        _characterItemDataColumns = new Dictionary<CharacterItemDataColumns, string>();
        sqlDatabase = new SqliteDatabase();
        sqlDatabase.CreateTable();  
    }

    /// <summary>
    /// 캐릭터 데이터 컬럼 반환
    /// </summary>
    /// <param name="columns">컬럼 키</param>
    /// <returns></returns>
    public string GetCharacterColumn(CharacterDataColumns columns)
    {
        if (!_characterDataColumns.ContainsKey(columns))
        {
            _characterDataColumns.TryAdd(columns, columns.ToString().ToLower());
        }   
        
        return _characterDataColumns[columns];
    }
    
    /// <summary>
    /// 캐릭터 소유 아이템 데이터 컬럼 반환
    /// </summary>
    /// <param name="columns">컬럼 키</param>
    /// <returns></returns>
    public string GetCharacterItemColumn(CharacterItemDataColumns columns)
    {
        if (!_characterItemDataColumns.ContainsKey(columns))
        {
            _characterItemDataColumns.TryAdd(columns, columns.ToString().ToLower());
        }
        
        return _characterItemDataColumns[columns];
    }

    /// <summary>
    /// 캐릭터 스킬 데이터 컬럼 반환
    /// </summary>
    /// <param name="columns">컬럼 키</param>
    /// <returns></returns>
    public string GetCharacterSkillColumn(CharacterSkillDataColumns columns)
    {
        if (!_characterSkillDataColumns.ContainsKey(columns))
        {
            _characterSkillDataColumns.TryAdd(columns, columns.ToString().ToLower());
        }
        
        return _characterSkillDataColumns[columns];
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
    public void UpdateCharacterDataColumn(string[] columnName, string[] columnValue, string condition, string conditionValue)
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

    /// <summary>
    /// 아이템 데이터 테이블 조회
    /// </summary>
    /// <param name="condition">조회 조건</param>
    /// <param name="conditionValue">조회 조건 값</param>
    /// <returns></returns>
    public IDataReader ReadItemDataColumn(string condition, string conditionValue)
    { 
        return sqlDatabase.ItemReadTable(condition, conditionValue);
    }

    /// <summary>
    /// 캐릭터 생성 초기 스킬 데이터 삽입
    /// </summary>
    /// <param name="slotID">현재 캐릭터 슬롯 ID</param> 
    public void InsertSkillDataColumn(string slotID)
    {
        sqlDatabase.SkillInsertTable(slotID);
    }

    /// <summary>
    /// 현재 슬롯의 캐릭터 스킬 데이터를 읽어옴
    /// </summary>
    /// <param name="slotID">현재 캐릭터 슬롯 ID</param>
    /// <returns></returns>
    public IDataReader ReadSkillDataColumn(string slotID)
    {
        return sqlDatabase.SkillReadTable(slotID);
    }

    /// <summary>
    /// 현재 슬롯의 캐릭터 스킬 데이터 업데이트
    /// </summary>
    /// <param name="column">캐릭터 스킬 테이블 컬럼 리스트</param>
    /// <param name="columnValue">업데이트 컬럼 리스트 데이터 값</param>
    /// <param name="condition">업데이트 진행할 조건, 캐릭터 슬롯 ID, 스킬 ID</param>
    /// <param name="conditionValue">슬롯의 Number, 업데이트 할 스킬 ID</param>
    public void UpdateSkillDataColumn(string[] column, string[] columnValue, string[] condition,
        string[] conditionValue)
    {
        sqlDatabase.SkillUpdateTable(column, columnValue, condition, conditionValue);   
    }
    
}