namespace Lynwood.Models
{
    /// <summary>
    /// Interface to implment on any Request Model that you would like to have hydrated by an Id parameter
    /// </summary>
    public interface IModelIdentifier
    {
        /// <summary>
        /// Nullable so that it is easier to put validation on the Request Models
        /// </summary>
        int? Id { get; set; }
    }
}