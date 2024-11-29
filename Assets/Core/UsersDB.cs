using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

namespace StairsCalc
{
    [CreateAssetMenu(fileName = "UsersDB", menuName = "StairsCalc/UsersDB", order = 0)]
    public class UsersDB : ScriptableObject
    {
        public List<AdminUser> admins;
        public List<User> users;
        
        [Button]
        public void AddAdmin(string id, string password)
        {
            AuthProvider admin = new AuthProvider(this);
            admin.AddAdmin(id, password);
        }
    }
}