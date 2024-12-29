import { ConsoleLogger, MiddlewareConsumer, Module } from "@nestjs/common";
import { ConfigModule, ConfigService } from "@nestjs/config";
import { TypeOrmModule } from "@nestjs/typeorm";
import { getTypeOrmConfig } from "./core/configs/typeorm.config";
import { FilesModule } from "./features/files/files.module";
import { ServeStaticModule } from "@nestjs/serve-static";
import { SERVER_FOLDER } from "./core/constants/default.constant";
import { UsersModule } from "./features/users/users.module";
import { PasswordModule } from "./features/password/password.module";
import { AuthModule } from "./features/auth/auth.module";
import { JwtModule } from "@nestjs/jwt";
import { getJwtConfig } from "./core/configs/jwt.config";
import { TokensModule } from "./features/tokens/tokens.module";
import { CookiesModule } from "./features/cookies/cookies.module";
import { HttpContextMiddleware } from "./core/moddlewares/http-context/http-context.middleware";
import { BearerStorageModule } from './features/bearer-storage/bearer-storage.module';

@Module({
  imports: [
    ConfigModule.forRoot(),
    JwtModule.registerAsync({
      imports: [ConfigModule],
      inject: [ConfigService],
      useFactory: getJwtConfig,
    }),
    ServeStaticModule.forRoot({
      rootPath: SERVER_FOLDER,
    }),
    TypeOrmModule.forRootAsync({
      imports: [ConfigModule],
      inject: [ConfigService],
      useFactory: getTypeOrmConfig,
    }),
    FilesModule,
    UsersModule,
    PasswordModule,
    AuthModule,
    TokensModule,
    CookiesModule,
    BearerStorageModule,
  ],
  providers: [ConsoleLogger],
})
export class AppModule {
  configure(consumer: MiddlewareConsumer): void {
    consumer.apply(HttpContextMiddleware).forRoutes("*");
  }
}
