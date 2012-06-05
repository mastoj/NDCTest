using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;

namespace NDCTest
{
    public class adding_999_with_1 : Specification<ThreeDigitCalculator, int>
    {
        protected override ThreeDigitCalculator Given()
        {
            return new ThreeDigitCalculator();
        }

        public override Command<int> When(ThreeDigitCalculator context)
        {
            return new AdditionCommand(context, 999, 1);
        }

        [Test]
        public void it_should_not_be_valid()
        {
            Assert.IsInstanceOf<OverflowException>(ThrownException);
        }

        [Test]
        public void it_should_be_5()
        {
            Assert.AreEqual(5, SUT.LastResult);
        }
    }

    class when_adding_2_with_2 : Specification<ThreeDigitCalculator, int>
    {
        protected override ThreeDigitCalculator Given()
        {
            return new ThreeDigitCalculator();
        }

        public override Command<int> When(ThreeDigitCalculator context)
        {
            return new AdditionCommand(context, 2, 2);
        }

        [Test]
        public void it_should_be_4()
        {
            Assert.AreEqual(4, Result);
        }
    }


    public class ThreeDigitCalculator
    {
        public int? LastResult { get; private set; }

        public int Add(int factor1, int factor2)
        {
            var result = factor1 + factor2;
            if(Math.Abs(result) > 999)
            {
                throw new OverflowException("Way too big for metro");
            }
            LastResult = result;
            return result;
        }

        public override string ToString()
        {
            return "Calculating";
        }
    }

    class AdditionCommand : Command<int>
    {
        private readonly ThreeDigitCalculator _threeDigitCalculator;

        public AdditionCommand(ThreeDigitCalculator threeDigitCalculator, int factor1, int factor2)
        {
            _threeDigitCalculator = threeDigitCalculator;
            Factor2 = factor2;
            Factor1 = factor1;
        }

        public int Factor1 { get; private set; }
        public int Factor2 { get; private set; }
        public override string ToString()
        {
            return string.Format("Adding numbers {0} and {1}", Factor1, Factor2);
        }

        public override int Execute()
        {
            return _threeDigitCalculator.Add(Factor1, Factor2);
        }
    }

}
