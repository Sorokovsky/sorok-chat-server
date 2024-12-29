import { ConsoleLogger, Module } from "@nestjs/common";
import { ConfigModule, ConfigService } from "@nestjs/config";
import { TypeOrmModule } from "@nestjs/typeorm";
import { getTypeOrmConfig } from "./core/configs/typeorm.config";
import { FilesModule } from "./features/files/files.module";
import { ServeStaticModule } from "@nestjs/serve-static";
import { SERVER_FOLDER } from "./core/constants/default.constant";
import { UsersModule } from './features/users/users.module';

@Module({
  imports: [
    ConfigModule.forRoot(),
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
  ],
  providers: [ConsoleLogger],
})
export class AppModule {}
