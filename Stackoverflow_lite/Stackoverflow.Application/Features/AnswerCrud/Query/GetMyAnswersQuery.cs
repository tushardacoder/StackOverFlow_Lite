using ErrorOr;
using MediatR;
using Stackoverflow.Application.DTOS;
using System;
using System.Collections.Generic;
using System.Text;

namespace Stackoverflow.Application.Features.AnswerCrud.Query
{
    public class GetMyAnswersQuery : IRequest<ErrorOr<List<AnswerDto>>>
    {
    }
}
