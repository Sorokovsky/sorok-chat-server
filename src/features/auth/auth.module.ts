import { Module } from "@nestjs/common";
import { AuthService } from "./auth.service";
import { AuthController } from "./auth.controller";
import { UsersModule } from "../users/users.module";
import { TokensModule } from "../tokens/tokens.module";
import { CookiesModule } from "../cookies/cookies.module";
import { BearerStorageModule } from "../bearer-storage/bearer-storage.module";

@Module({
  imports: [UsersModule, TokensModule, CookiesModule, BearerStorageModule],
  controllers: [AuthController],
  providers: [AuthService],
})
export class AuthModule {}
