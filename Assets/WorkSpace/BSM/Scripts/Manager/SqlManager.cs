using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using UnityEngine;
using Zenject;

public class SqlManager : IInitializable
{
    public SqliteDatabase SqlDatabase;
    public IDataReader DataReader;
    
    public void Initialize()
    {
        Debug.Log("테이블 생성");
        SqlDatabase = new SqliteDatabase(); 
        SqlDatabase.CreateTable();
        
    }

    public void ReadDataColumn<T>(string columnName, string where = "", T whereValue = default(T))
    {
        IDataReader reader = SqlDatabase.ReadTable(columnName, where, whereValue);
        
        while (reader.Read())
        {
            Debug.Log(reader.GetString(0));
        }
    }
    
}
