using FluentResults;
using MediatR;

namespace Filmio.BLL.MediatR.Authentication.DeleteAccount;

public record DeleteAccountCommand(int userId)
    : IRequest<Result<Unit>>;