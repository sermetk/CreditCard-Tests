using CreditCards.Controllers;
using CreditCards.Core.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Moq;
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
    }
}
