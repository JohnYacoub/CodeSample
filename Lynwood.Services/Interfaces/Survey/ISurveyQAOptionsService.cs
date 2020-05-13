using System.Collections.Generic;
using Lynwood.Models.Domain;
using Lynwood.Models.Requests.SurveyQuestionAnswerOptions;
using Lynwood.Models.Requests.Surveys;

namespace Lynwood.Services.SurveyQuestionAnswerOptions
{
    public interface ISurveyQAOptionsService
    {
        void Delete(int id);
        int Add(SurveyQuestionAnswerOptionsAddRequest model, int userId);
        List<SurveyQuestionAnswerOption> Get();
        SurveyQuestionAnswerOption GetById(int id);
        void Update(SurveyQuestionAnswerOptionsUpdateRequest model, int userId);
    }
}