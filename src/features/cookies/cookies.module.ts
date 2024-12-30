import { Module } from "@nestjs/common";
import { CookiesService } from "@features/cookies/cookies.service";

@Module({
  providers: [CookiesService],
  exports: [CookiesService],
})
export class CookiesModule {}
