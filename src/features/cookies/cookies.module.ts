import { Module } from "@nestjs/common";
import { CookiesService } from "./cookies.service";
import { ExecutionContextHost } from "@nestjs/core/helpers/execution-context-host";

@Module({
  providers: [CookiesService, ExecutionContextHost],
  exports: [CookiesService],
})
export class CookiesModule {}
