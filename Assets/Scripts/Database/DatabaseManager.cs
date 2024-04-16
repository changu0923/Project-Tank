using MySqlConnector;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Security.Cryptography;
using System.Text;
using Unity.VisualScripting;
using UnityEngine;

public class DatabaseManager : MonoBehaviour
{
    #region Singleton
    private static DatabaseManager instance;
    public static DatabaseManager Instance
    {
        get
        {
            if (instance == null)
            {
                instance = FindObjectOfType<DatabaseManager>();
                if (instance == null)
                {
                    GameObject obj = new GameObject("DatabaseManager");
                    instance = obj.AddComponent<DatabaseManager>();
                }
            }
            return instance;
        }
    }    
    #endregion

    [SerializeField] string passwd;
    private MySqlConnection conn;

    private UserData currentUserdata;
    public UserData CurrentUserdata { get => currentUserdata; set => currentUserdata = value; }


    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        ConnectDatabase();
    }

    private void ConnectDatabase()
    {
        try
        {
            string config = $"server=localhost;port=3306;database=project_tank;uid=root;pwd={passwd};charset=utf8;";
            conn = new MySqlConnection(config);
            conn.Open();
            Debug.Log($"Mysql Connect Success : [{DateTime.Now}]");
        }
        catch (Exception e)
        {
            Debug.LogError(e.Message);
        }
    }

    public bool Login(string id, string pw)
    {
        MySqlCommand cmd = new MySqlCommand();
        cmd.Connection = conn;
        cmd.CommandText = $"SELECT * FROM users WHERE user_email =\'{id}\' AND user_password = \'{pw}\'";
        MySqlDataAdapter da = new MySqlDataAdapter(cmd);
        DataSet dataset = new DataSet();
        da.Fill(dataset);
        bool isLogin = dataset.Tables.Count > 0 && dataset.Tables[0].Rows.Count > 0;
        if (isLogin)
        {
            print($"Login Success [{DateTime.Now}]");
            // TODO get userinfo
            CurrentUserdata = GetUserData(id);
            return true;
        }
        else
        {
            print($"Login Failed [{DateTime.Now}]");
            return false;
        }
    }

    public bool CreateUser(UserData userData)
    {
        MySqlCommand cmd = new MySqlCommand();
        cmd.Connection = conn;

        cmd.CommandText = $"INSERT INTO users (uid, user_email, user_password, user_nickname) VALUES ('{userData.Uid}', '{userData.UserEmail}', '{userData.UserPassword}', '{userData.UserNickname}')";
        try
        {
            int count = cmd.ExecuteNonQuery();
            if (count != 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        catch (Exception e)
        {
            Debug.LogError(e.Message);
            return false;
        }
    }

    public bool UpdateNickname(string nickname)
    {       
        string uid = currentUserdata.Uid;
        MySqlCommand cmd = new MySqlCommand();
        cmd.Connection = conn;
        cmd.CommandText = $"UPDATE users SET user_nickname = '{nickname}' WHERE uid = '{uid}';";
        int result = cmd.ExecuteNonQuery();
        CurrentUserdata = GetUserData(currentUserdata.UserEmail);
        try
        { 
            if (result != 0)
            {                
                return true;
            }
            else
            {
                return false;
            }
        }
        catch (Exception e)
        {
            Debug.LogError(e.Message);
            return false;
        }
    }

    private UserData GetUserData(string id)
    {
        UserData returnData = new UserData();

        MySqlCommand cmd = new MySqlCommand();
        cmd.Connection = conn;
        cmd.CommandText = $"SELECT * FROM users WHERE user_email =\'{id}\';";
        MySqlDataAdapter da = new MySqlDataAdapter(cmd);
        DataSet dataset = new DataSet();
        da.Fill(dataset);

        if (dataset.Tables.Count > 0)
        {
            DataTable dataTable = dataset.Tables[0];

            foreach (DataRow row in dataTable.Rows)
            {
                foreach (DataColumn col in dataTable.Columns)
                {
                    // print($"{col.ColumnName}: {row[col]}");
                    if (col.ColumnName == "uid")
                    {
                        returnData.Uid = row[col].ToString();
                    }
                    else if (col.ColumnName == "user_email")
                    {
                        returnData.UserEmail = row[col].ToString();
                    }
                    else if (col.ColumnName == "user_password")
                    {
                        returnData.UserPassword = row[col].ToString();
                    }
                    else if (col.ColumnName == "user_nickname")
                    {
                        returnData.UserNickname = row[col].ToString();  
                    }
                    else if(col.ColumnName == "user_level")
                    {
                        returnData.UserLevel = int.Parse(row[col].ToString());
                    }
                }
            }
        }
        else
        {
            print("No data found.");
        }
        return returnData;
    }

    public int GetUserLevel(string nickName)
    {
        MySqlCommand cmd = new MySqlCommand();
        cmd.Connection = conn;
        cmd.CommandText = $"SELECT user_level FROM users WHERE user_nickname =\'{nickName}\';";
        MySqlDataAdapter da = new MySqlDataAdapter(cmd);
        DataSet dataset = new DataSet();
        da.Fill(dataset);

        if (dataset.Tables.Count > 0)
        {
            DataTable dataTable = dataset.Tables[0];

            foreach (DataRow row in dataTable.Rows)
            {
                foreach (DataColumn col in dataTable.Columns)
                {
                    if (col.ColumnName == "user_level")
                    {
                        return int.Parse(row[col].ToString());
                    }
                }
            }
        }
        return 1;
    }

    public bool CheckID(string id)
    {
        MySqlCommand cmd = new MySqlCommand();
        cmd.Connection = conn;
        cmd.CommandText = $"SELECT * FROM users WHERE user_email =\'{id}\';";
        MySqlDataAdapter da = new MySqlDataAdapter(cmd);
        DataSet dataset = new DataSet();
        da.Fill(dataset);
        bool isExists = dataset.Tables.Count > 0 && dataset.Tables[0].Rows.Count > 0;
        return isExists;
    }
}
