import { Injectable } from "@nestjs/common";
import dayjs from "dayjs";
import { Request, Response } from "express";
import { getHttpContext } from "../../utils/http-context";

@Injectable()
export class CookiesService {
  private readonly request: Request;
  private readonly response: Response;
  constructor() {
    const { request, response } = getHttpContext();
    this.request = request;
    this.response = response;
  }

  public async setCookie(name: string, value: string): Promise<void> {
    this.response.cookie(name, value, {
      httpOnly: true,
      maxAge: dayjs().add(7, "day").valueOf(),
    });
  }

  public async getCookie(name: string): Promise<string | null> {
    return this.request.cookies[name];
  }

  public async deleteCookie(name: string): Promise<void> {
    this.response.cookie(name, "", {
      httpOnly: true,
      maxAge: 0,
    });
  }
}
