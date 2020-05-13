using System;
using System.Collections.Generic;
using Lynwood.Models;
using Lynwood.Models.Domain;
using Lynwood.Models.Domain.Surveys;
using Lynwood.Models.Requests;
using Lynwood.Models.Requests.Surveys;

namespace Lynwood.Services
{
    public interface ISurveysService
    {
        void Delete(int id);
        int Add(SurveysAddRequest model, int id);
        List<Survey> Get();
        Survey GetById(int id);
        void Update(SurveysUpdateRequest model, int id);

        #region Survey Instance
        void DeleteInstance(int id);
        int AddInstance(SurveyInstancesAddRequest model);
        List<SurveyInstance> GetInstances();
        SurveyInstance GetByInstanceId(int id);
        void UpdateInstance(SurveyInstancesUpdateRequest model);
        SurveyInstanceQADetails GetInstanceDetailsById(int id);
        Paged<SurveyInstanceDetails> GetInstancesPaginated(int pageIndex, int pageSize);
        Paged<SurveyInstanceDetails> GetInstancesPaginated(int id, int pageIndex, int pageSize);

        Paged<SurveyInstanceDetails> GetInstancesByDatePaginated(DateTime dateCreated, int pageIndex, int pageSize);
        List<SurveyInstancesTotalByMonth> GetTotalMonthlyCount(int year);
        #endregion

        #region Survey Section
        int AddSection(SurveySectionsAddRequest model);
        void DeleteSection(int Id);
        void UpdateSection(SurveySectionsUpdateRequest model);
        SurveySection SelectSectionById(int Id);
        List<SurveySection> SelectAllSections();
        SurveyDetails GetDetailsBySurveyId(int id);
        #endregion

        #region Survey Status
        List<SurveyStatus> GetStatus();
        #endregion

        #region Survey Types
        List<SurveyType> GetTypes();
        #endregion

        #region Chart Years
        List<ChartYears> GetYears();
        #endregion
    }
}