import { Global, Module } from "@nestjs/common";
import { AuthService } from "@features/auth/auth.service";
import { AuthController } from "@features/auth/auth.controller";
import { UsersModule } from "@features/users/users.module";
import { TokensModule } from "@features/tokens/tokens.module";
import { CookiesModule } from "@features/cookies/cookies.module";
import { BearerStorageModule } from "@features/bearer-storage/bearer-storage.module";
import { PasswordModule } from "@features/password/password.module";

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
export class AuthModule {
}
