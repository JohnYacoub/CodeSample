using Lynwood.Models.Domain;
using Lynwood.Models.Requests.SurveyAnswers;
using System.Collections.Generic;

namespace Lynwood.Services
{
    public interface ISurveyAnswersServices
    {
        int Insert(SurveyAnswersAddRequest model, int userId);
        void Delete(int id);
        void Update(UpdateSurveyAnswers model, int userId);
        SurveyAnswer Get(int id);
        List<SurveyAnswer> Get();
        SurveyDisplayAnswers GetAnswersByInstanceId(int id);
        void Insert_Multiple(List<SurveyAnswersAddRequest> model);
    }
}