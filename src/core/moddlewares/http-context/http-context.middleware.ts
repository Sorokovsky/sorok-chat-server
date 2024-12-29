import { Injectable, NestMiddleware } from "@nestjs/common";
import { setHttpContext } from "../../../utils/http-context";
import { HttpContextDto } from "../../contracts/http-context.dto";
import { Request, Response } from "express";

@Injectable()
export class HttpContextMiddleware implements NestMiddleware {
  use(req: Request, res: Response, next: () => void): void {
    setHttpContext(new HttpContextDto(req, res));
    next();
  }
}
