namespace Data.Common.Contracts.Entities
{
    //[Table("RoleUsers")]
    public class UserRole
    {
        //[Column("User_Id")]
        public string UserId { get; set; }
        public User User { get; set; }

        //[Column("Role_Id")]
        public string RoleId { get; set; }
        public Role Role { get; set; }
    }
}
