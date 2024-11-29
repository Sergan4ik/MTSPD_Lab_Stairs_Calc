namespace StairsCalc
{
    public interface IAuthProvider
    {
        public bool Login(string emailOrId, string password);
        public bool Register(string email, string password);
    }
    
    public interface IAuthAdminProvider
    {
        public void AddAdmin(string id, string password);
        public bool IsAdmin(string emailOrId);
        public bool LoginAdmin(string id, string password);
    }
    public class AuthProvider : IAuthProvider, IAuthAdminProvider
    {
        private readonly UsersDB _db;

        public AuthProvider(UsersDB db)
        {
            _db = db;
        }
        
        public bool Login(string emailOrId, string password)
        {
            foreach (var user in _db.users)
            {
                if (user.email == emailOrId || user.Id == emailOrId)
                {
                    return user.passwordHash == password.GetHashCode();
                }
            }

            return false;
        }
        
        public bool IsAdmin(string emailOrId)
        {
            foreach (var admin in _db.admins)
            {
                if (admin.Id == emailOrId)
                {
                    return true;
                }
            }

            return false;
        }
        
        public bool Register(string email, string password)
        {
            foreach (var user in _db.users)
            {
                if (user.email == email)
                {
                    return false;
                }
            }

            _db.users.Add(new User
            {
                Id = email,
                email = email,
                passwordHash = password.GetHashCode()
            });
            return true;
        }
        
        public void AddAdmin(string id, string password)
        {
            _db.admins.Add(new AdminUser
            {
                Id = id,
                passwordHash = password.GetHashCode()
            });
        }
        
        public bool LoginAdmin(string id, string password)
        {
            foreach (var admin in _db.admins)
            {
                if (admin.Id == id)
                {
                    return admin.passwordHash == password.GetHashCode();
                }
            }

            return false;
        }
    }
}