﻿using Microsoft.Extensions.DependencyInjection;
using Xunit;

namespace HotChocolate.Validation
{
    public class RequiredArgumentRuleTests
        : DocumentValidatorVisitorTestBase
    {
        public RequiredArgumentRuleTests()
            : base(builder => builder.AddArgumentRules())
        {
        }

        [Fact]
        public void BooleanArgFieldAndNonNullBooleanArgField()
        {
            ExpectValid(@"
                query {
                    arguments {
                        ... goodBooleanArg
                        ... goodNonNullArg
                    }
                }

                fragment goodBooleanArg on Arguments {
                    booleanArgField(booleanArg: true)
                }

                fragment goodNonNullArg on Arguments {
                    nonNullBooleanArgField(nonNullBooleanArg: true)
                }
            ");
        }

        [Fact]
        public void GoodBooleanArgDefault()
        {
            ExpectValid(@"
                query {
                    arguments {
                        ... goodBooleanArgDefault
                    }
                }

                fragment goodBooleanArgDefault on Arguments {
                    booleanArgField
                }
            ");
        }

        [Fact]
        public void MissingRequiredArg()
        {
            // arrange
            ExpectErrors(@"
                query {
                    arguments {
                        ... missingRequiredArg
                    }
                }

                fragment missingRequiredArg on Arguments {
                    nonNullBooleanArgField
                }
            ",
            t => Assert.Equal(
                $"The argument `nonNullBooleanArg` is required.", t.Message));
        }

        [Fact]
        public void MissingRequiredArgNonNullBooleanArg()
        {
            ExpectErrors(@"
                query {
                    arguments {
                        ... missingRequiredArg
                    }
                }

                fragment missingRequiredArg on Arguments {
                    nonNullBooleanArgField(nonNullBooleanArg: null)
                }
            ",
            t => Assert.Equal(
                $"The argument `nonNullBooleanArg` is required.", t.Message));
        }

        [Fact]
        public void MissingRequiredDirectiveArg()
        {
            ExpectErrors(@"
                query {
                    arguments {
                        ... missingRequiredArg
                    }
                }

                fragment missingRequiredArg on Arguments {
                    nonNullBooleanArgField(nonNullBooleanArg: true) @skip()
                }
            ",
            t => Assert.Equal(
                $"The argument `if` is required.", t.Message));
        }

        [Fact]
        public void BadMultipleNullValueType()
        {
            ExpectErrors(@"
                 {
                     arguments {
                         multipleReqs(x: 1, y: null)
                     }
                 }
             ");
        }

        [Fact]
        public void BadNullIntoNonNullBool()
        {
            ExpectErrors(@"
                {
                    arguments {
                        nonNullBooleanArgField(nonNullBooleanArg: null)
                    }
                }
            ");
        }

        [Fact]
        public void BadNullIntoNonNullFloat()
        {
            ExpectErrors(@"
                {
                    arguments {
                        nonNullFloatArgField(floatArg: null)
                    }
                }
            ");
        }

        [Fact]
        public void BadNullIntoNonNullId()
        {
            ExpectErrors(@"
                {
                    arguments {
                        nonNullIdArgField(idArg: null)
                    }
                }
            ");
        }

        [Fact]
        public void BadNullIntoNonNullInt()
        {
            ExpectErrors(@"
                {
                    arguments {
                        nonNullIntArgField(intArg: null)
                    }
                }
            ");
        }

        [Fact]
        public void BadNullIntoNonNullString()
        {
            ExpectErrors(@"
                {
                    arguments {
                        nonNullStringArgField(stringArg: null)
                    }
                }
            ");
        }
    }
}