import { Module } from "@nestjs/common";
import { PasswordService } from "@features/password/password.service";
import { ConfigModule } from "@nestjs/config";

@Module({
  providers: [PasswordService],
  imports: [ConfigModule],
  exports: [PasswordService],
})
export class PasswordModule {}
