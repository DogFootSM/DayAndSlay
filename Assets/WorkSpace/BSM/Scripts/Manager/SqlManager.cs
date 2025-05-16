using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using UnityEngine;
using Zenject;

public class SqlManager : IInitializable
{
    private SqliteDatabase sqlDatabase;
  
    /// <summary>
    /// 게임 시작 시 테이블 생성
    /// </summary>
    public void Initialize()
    {
        sqlDatabase = new SqliteDatabase(); 
        sqlDatabase.CreateTable(); 
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
        return sqlDatabase.ReadTable(columnName, where, whereValue, operation);
    }
    
}
