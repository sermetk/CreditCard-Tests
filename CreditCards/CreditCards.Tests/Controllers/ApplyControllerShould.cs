using CreditCards.Controllers;
using CreditCards.Core.Interfaces;
using CreditCards.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Moq;
using System.Threading.Tasks;
using Xunit;

namespace CreditCards.Tests.Controllers
{
    public class ApplyControllerShould
    {
        private readonly Mock<ICreditCardApplicationRepository> _mockRepository;
        // sut => System Under Test
        private readonly ApplyController _sut;

        public ApplyControllerShould()
        {
            _mockRepository = new Mock<ICreditCardApplicationRepository>();
            _sut = new ApplyController(_mockRepository.Object);
        }

        [Fact]
        public void ReturnViewForIndex()
        {
            var result = _sut.Index();

            Assert.IsType<ViewResult>(result);
        }

        [Fact]
        public async Task ReturnViewWhenInvalidModelState()
        {
            _sut.ModelState.AddModelError("x", "Test Error");

            var application = new NewCreditCardApplicationDetails
            {
                FirstName = "Sarah"
            };

            var result = await _sut.Index(application);

            var viewResult = Assert.IsType<ViewResult>(result);

            var model = Assert.IsType<NewCreditCardApplicationDetails>(viewResult.Model);

            Assert.Equal(application.FirstName, model.FirstName);
        }
    }
}
