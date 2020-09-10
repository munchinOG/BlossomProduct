using BlossomProduct.Core.Models;
using BlossomProduct.Core.Models.Repo;
using Microsoft.AspNetCore.Mvc;

namespace BlossomProduct.Controllers
{
    public class FeedbackController : Controller
    {
        private readonly IFeedbackRepository _feedbackRepository;

        public FeedbackController( IFeedbackRepository feedbackRepository )
        {
            _feedbackRepository = feedbackRepository;
        }

        [HttpGet]
        public IActionResult Index( )
        {
            return View( "Feedback" );
        }

        [HttpPost]
        public IActionResult Index( Feedback feedback )
        {
            if(ModelState.IsValid)
            {
                _feedbackRepository.AddFeedback( feedback );
                return RedirectToAction( "FeedbackComplete" );
            }

            return View( "Feedback" );
        }

        public IActionResult FeedbackComplete( )
        {
            return View();
        }
    }
}
