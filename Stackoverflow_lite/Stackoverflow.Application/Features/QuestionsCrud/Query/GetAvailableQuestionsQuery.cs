using MediatR;
using Stackoverflow.Application.DTOS;
using System;
using System.Collections.Generic;
using System.Text;

namespace Stackoverflow.Application.Features.QuestionsCrud.Query
{
    public class GetAvailableQuestionsQuery : IRequest<List<QuestionDto>>
    {

    }
}
