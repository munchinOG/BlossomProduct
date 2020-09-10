using BlossomProduct.Core.EFContext;

namespace BlossomProduct.Core.Models.Repo
{
    public class FeedbackRepository : IFeedbackRepository
    {
        private readonly BlossomDbContext _blossomDbContext;

        public FeedbackRepository( BlossomDbContext blossomDbContext )
        {
            _blossomDbContext = blossomDbContext;
        }
        public void AddFeedback( Feedback feedback )
        {
            _blossomDbContext.Feedbacks.Add( feedback );
            _blossomDbContext.SaveChanges();
        }
    }
}
