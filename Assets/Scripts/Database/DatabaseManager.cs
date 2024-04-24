using MySqlConnector;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Reflection;
using System.Security.Cryptography;
using System.Text;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem.XR.Haptics;
using UnityEngine.ProBuilder.Shapes;

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

    private Dictionary<int, TankData> vehiclesInDB = new Dictionary<int, TankData>();
    private UserData currentUserdata;
    private List<TankData> currentUserOwnedVehicles = new();
    private TankData selectedTank;
    public UserData CurrentUserdata { get => currentUserdata; set => currentUserdata = value; }
    public List<TankData> CurrentUserOwnedVehicles { get => currentUserOwnedVehicles; }
    public TankData SelectedTank { get => selectedTank; set => selectedTank = value; }

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
            GetVehiclesFromDB();
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
            CurrentUserdata = GetUserData(id);
            GetUserOwnedVehicles(CurrentUserdata.Uid);
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
                UserData tempUser = GetUserData(userData.UserEmail);
                AddStarterKit(tempUser.Uid);
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

    public void AddStarterKit(string uid)
    {
        MySqlCommand cmd = new MySqlCommand();
        cmd.Connection = conn;
        cmd.CommandText = $"INSERT INTO user_owned_tanks (uid, tank_id) VALUES ('{uid}', '1')";
        cmd.ExecuteNonQuery();
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
                    else if(col.ColumnName == "silver")
                    {
                        returnData.Silver = int.Parse(row[col].ToString());
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

    public void GetUserOwnedVehicles(string uid)
    {
        MySqlCommand cmd = new MySqlCommand();
        cmd.Connection = conn;
        cmd.CommandText = $"SELECT tank_id FROM user_owned_tanks WHERE uid = '{uid}'";
        MySqlDataAdapter da = new MySqlDataAdapter(cmd);
        DataSet dataset = new DataSet();
        da.Fill(dataset);

        if (dataset.Tables.Count > 0)
        {
            DataTable dataTable = dataset.Tables[0];

            foreach (DataRow row in dataTable.Rows)
            {
                TankData tankData = new TankData();
                foreach (DataColumn col in dataTable.Columns)
                {
                    if (col.ColumnName == "tank_id")
                    {
                        int tankID = int.Parse(row[col].ToString());
                        if (vehiclesInDB.ContainsKey(tankID))
                        {
                            TankData originalTankData = vehiclesInDB[tankID];
                            tankData.TankID = originalTankData.TankID;
                            tankData.TankName = originalTankData.TankName;
                            tankData.TankNation = originalTankData.TankNation;
                            tankData.TankPrice = originalTankData.TankPrice;
                            tankData.TankDescription = originalTankData.TankDescription;
                        }
                    }
                    else if(col.ColumnName == "item_slot_1")
                    {
                        tankData.ItemSlot_01 = int.Parse(row[col].ToString());
                    }
                    else if (col.ColumnName == "item_slot_2")
                    {
                        tankData.ItemSlot_02 = int.Parse(row[col].ToString());
                    }
                    else if (col.ColumnName == "item_slot_3")
                    {
                        tankData.ItemSlot_03 = int.Parse(row[col].ToString());                        
                    }
                    else if (col.ColumnName =="camo_slot")
                    {
                        tankData.CamoSlot = int.Parse(row[col].ToString());
                    }
                }
                currentUserOwnedVehicles.Add(tankData);
            }
        }
        else
        {
            print("No data found.");
        }

    }

    public void GetVehiclesFromDB()
    {
        MySqlCommand cmd = new MySqlCommand();
        cmd.Connection = conn;
        cmd.CommandText = $"SELECT * FROM tanks";
        MySqlDataAdapter da = new MySqlDataAdapter(cmd);
        DataSet dataset = new DataSet();
        da.Fill(dataset);

        if (dataset.Tables.Count > 0)
        {
            DataTable dataTable = dataset.Tables[0];

            foreach (DataRow row in dataTable.Rows)
            {
                TankData tankData = new TankData();
                int tankIndex = 0;
                foreach (DataColumn col in dataTable.Columns)
                {
                    if (col.ColumnName == "tank_id")
                    {
                        tankData.TankID = int.Parse(row[col].ToString());
                        tankIndex = tankData.TankID;
                    }
                    else if(col.ColumnName == "tank_name")
                    {
                        tankData.TankName = row[col].ToString();
                    }
                    else if (col.ColumnName == "tank_nation")
                    {
                        tankData.TankNation = row[col].ToString();
                    }
                    else if (col.ColumnName == "tank_price")
                    {
                        tankData.TankPrice = int.Parse(row[col].ToString());
                    }
                    else if (col.ColumnName == "description")
                    {
                        tankData.TankDescription = row[col].ToString();
                    }
                }
                vehiclesInDB.Add(tankIndex, tankData);
            }
        }
        else
        {
            print("No data found.");
        }
    }

    public bool CheckID(string email)
    {
        MySqlCommand cmd = new MySqlCommand();
        cmd.Connection = conn;
        cmd.CommandText = $"SELECT * FROM users WHERE user_email =\'{email}\';";
        MySqlDataAdapter da = new MySqlDataAdapter(cmd);
        DataSet dataset = new DataSet();
        da.Fill(dataset);
        bool isExists = dataset.Tables.Count > 0 && dataset.Tables[0].Rows.Count > 0;
        return isExists;
    }

    public bool ChangeNickname(string newName)
    {
        MySqlCommand cmd = new MySqlCommand();
        cmd.Connection = conn;
        cmd.CommandText = $"UPDATE users SET user_nickname = '{newName}' WHERE uid = '{currentUserdata.Uid}';";
        int result = cmd.ExecuteNonQuery();

        if(result !=0)
        {
            return true;
        }
        else
        {
            return false;
        }    
    }

    public bool ChangeCamo(int index)
    {
        MySqlCommand cmd = new MySqlCommand();
        cmd.Connection = conn;
        cmd.CommandText = $"UPDATE user_owned_tanks SET camo_slot = '{index}' WHERE uid = '{currentUserdata.Uid}' AND tank_id = '{selectedTank.TankID}';";
        int result = cmd.ExecuteNonQuery();

        if (result != 0)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    public TankData UpdateTankData(int getTank_ID)
    {
        MySqlCommand cmd = new MySqlCommand();
        cmd.Connection = conn;
        cmd.CommandText = $"SELECT * FROM user_owned_tanks WHERE uid = '{currentUserdata.Uid}' AND tank_id = '{getTank_ID}';";
        MySqlDataAdapter da = new MySqlDataAdapter(cmd);
        DataSet dataset = new DataSet();
        da.Fill(dataset);

        if (dataset.Tables.Count > 0)
        {
            DataTable dataTable = dataset.Tables[0];

            foreach (DataRow row in dataTable.Rows)
            {
                TankData tankData = new TankData();
                foreach (DataColumn col in dataTable.Columns)
                {
                    if (col.ColumnName == "tank_id")
                    {
                        tankData.TankID = int.Parse(row[col].ToString());
                    }
                    else if (col.ColumnName == "item_slot_1")
                    {
                        tankData.ItemSlot_01 = int.Parse(row[col].ToString());
                    }
                    else if (col.ColumnName == "item_slot_2")
                    {
                        tankData.ItemSlot_02 = int.Parse(row[col].ToString());
                    }
                    else if (col.ColumnName == "item_slot_3")
                    {
                        tankData.ItemSlot_03 = int.Parse(row[col].ToString());
                    }
                    else if (col.ColumnName == "camo_slot")
                    {
                        tankData.CamoSlot = int.Parse(row[col].ToString());
                    }
                }
                return tankData;
            }
        }
        return null;
    }
}
