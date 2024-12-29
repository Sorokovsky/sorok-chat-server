import { Module } from "@nestjs/common";
import { PasswordService } from "./password.service";
import { ConfigModule } from "@nestjs/config";

@Module({
  providers: [PasswordService],
  imports: [ConfigModule],
  exports: [PasswordService],
})
export class PasswordModule {}
