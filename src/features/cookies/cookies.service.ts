import { Injectable } from "@nestjs/common";
import dayjs from "dayjs";
import { Request, Response } from "express";

@Injectable()
export class CookiesService {
  public async setCookie(
    response: Response,
    name: string,
    value: string,
  ): Promise<void> {
    response.cookie(name, value, {
      httpOnly: true,
      maxAge: dayjs().add(7, "day").valueOf(),
    });
  }

  public async getCookie(
    request: Request,
    name: string,
  ): Promise<string | null> {
    return request.cookies[name] ?? null;
  }

  public async deleteCookie(response: Response, name: string): Promise<void> {
    response.cookie(name, "", {
      httpOnly: true,
      maxAge: 0,
    });
  }
}
