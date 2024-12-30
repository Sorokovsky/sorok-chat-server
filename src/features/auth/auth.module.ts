import { Global, Module } from "@nestjs/common";
import { AuthService } from "./auth.service";
import { AuthController } from "./auth.controller";
import { UsersModule } from "../users/users.module";
import { TokensModule } from "../tokens/tokens.module";
import { CookiesModule } from "../cookies/cookies.module";
import { BearerStorageModule } from "../bearer-storage/bearer-storage.module";
import { PasswordModule } from "../password/password.module";

@Global()
@Module({
  imports: [
    UsersModule,
    TokensModule,
    CookiesModule,
    BearerStorageModule,
    PasswordModule,
  ],
  controllers: [AuthController],
  providers: [AuthService],
  exports: [AuthService],
})
export class AuthModule {}
