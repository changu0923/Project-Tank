using System;
using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Text;
using UnityEngine;

[Serializable]
public class UserData
{
    private string uid;
    private string userEmail;
    private string userPassword;
    private string userNickname;
    private int    userLevel;
    private int    silver;
    public string Uid { get { return uid; } set => uid = value; }
    public string UserEmail { get { return userEmail; } set => userEmail = value; }
    public string UserPassword { get { return userPassword; } set => userPassword = value; }
    public string UserNickname { get => userNickname; set => userNickname = value; }
    public int UserLevel { get => userLevel; set => userLevel = value; }
    public int Silver { get => silver; set => silver = value; }

    public UserData() { }

    public UserData(string email, string passwd)
    {
        Guid uuid = Guid.NewGuid();
        uid = uuid.ToString();
        userEmail = email;
        userPassword = HashPassword(passwd);

        string[] splits = email.Split('@');
        userNickname = splits[0];
    }
    public static string HashPassword(string password)
    {
        using (SHA256 sha256 = SHA256.Create())
        {
            byte[] bytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(password));
            StringBuilder sb = new StringBuilder();
            for (int i = 0; i < bytes.Length; i++)
            {
                sb.Append(bytes[i].ToString("x2"));
            }
            return sb.ToString();
        }
    }
}
