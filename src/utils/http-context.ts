import { HttpContextDto } from "../core/contracts/http-context.dto";

let httpContext: HttpContextDto = {} as HttpContextDto;

export const setHttpContext = (context: HttpContextDto): void => {
  httpContext = context;
};

export const getHttpContext = (): HttpContextDto => httpContext;
