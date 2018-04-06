using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Security.Cryptography;
using System;
using System.Text;

public class Id : MonoBehaviour
{

    // Use this for initialization
    void Start()
    {
        SetIdIfNotSet();
    }

    void SetIdIfNotSet()
    {
        if (!PlayerPrefs.HasKey("id"))
        {
            int id = (int)(UnityEngine.Random.Range(1, 100000));
            string firstDate = DateTime.Now.ToString();

            PlayerPrefs.SetInt("id", id);
            PlayerPrefs.SetString("firstDate", firstDate);
            MD5CryptoServiceProvider md5 = new MD5CryptoServiceProvider();

            byte[] md5Bytes = md5.ComputeHash(System.Text.Encoding.ASCII.GetBytes(id.ToString() + firstDate));
            StringBuilder sBuilder = new StringBuilder();
            for (int i = 0; i < md5Bytes.Length; i++)
            {
                sBuilder.Append(md5Bytes[i].ToString("x2"));
            }

            PlayerPrefs.SetString("idHash", sBuilder.ToString());
        }
    }

}
