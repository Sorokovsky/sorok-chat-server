import { Injectable } from "@nestjs/common";
import * as dayjs from "dayjs";
import { getHttpContext } from "../../utils/http-context";

@Injectable()
export class CookiesService {
  public async setCookie(name: string, value: string): Promise<void> {
    const { response } = getHttpContext();
    response.cookie(name, value, {
      httpOnly: true,
      maxAge: dayjs().add(7, "day").valueOf(),
    });
  }

  public async getCookie(name: string): Promise<string | null> {
    const { request } = getHttpContext();
    console.log(request.url);
    return request.cookies[name] ?? null;
  }

  public async deleteCookie(name: string): Promise<void> {
    const { response } = getHttpContext();
    response.cookie(name, "", {
      httpOnly: true,
      maxAge: 0,
    });
  }
}
