using System;

namespace Lynwood.Models.Domain
{
    public class SurveyInstanceDetails
    {
        public int SurveyId  { get; set; }
        public string SurveyName  { get; set; }
        public int InstanceId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string AvatarUrl { get; set; }
        public string Email { get; set; }
        public DateTime DateCreated { get; set; }
    }
}
