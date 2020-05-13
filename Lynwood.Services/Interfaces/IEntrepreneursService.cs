using Lynwood.Models;
using Lynwood.Models.Domain;
using Lynwood.Models.Requests.Entrepreneurs;
using System.Collections.Generic;

namespace Lynwood.Services.Interfaces
{
    public interface IEntrepreneursService
    {
        int Insert(EntrepreneursAddRequest model, int userId);
        void Update(EntrepreneursUpdateRequest model, int userId);
        Entrepreneur GetById(int id);
        List<Entrepreneur> GetAll();
        void Delete(int id);
        Paged<Entrepreneur> GetAllByPagination(int pageIndex, int pageSize);
        Paged<Entrepreneur> SearchPagination(int pageIndex, int pageSize, string query);
        EntrepreneursTypes GetAllOptions();
    }
}