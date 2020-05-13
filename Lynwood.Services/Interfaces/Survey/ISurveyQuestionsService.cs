using System.Collections.Generic;
using Lynwood.Models.Domain;
using Lynwood.Models.Requests.SurveyQuestions;

namespace Lynwood.Services.SurveyQuestions
{
    public interface ISurveyQuestionsService
    {
        void Delete(int id);
        int Add(SurveyQuestionsAddRequest model, int submitId);
        List<SurveyQuestion> Get();
        SurveyQuestion GetById(int id);
        void Update(SurveyQuestionsUpdateRequest model, int Id);
    }
}