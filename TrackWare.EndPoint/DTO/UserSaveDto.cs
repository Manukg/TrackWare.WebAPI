namespace TrackWare.EndPoint.DTO
{
    public class UserSaveDto
    {
        public string LoginID { get; set; }
        public string TypeCode { get; set; }

        public IFormFile UserPhoto { get; set; }
    }
}
