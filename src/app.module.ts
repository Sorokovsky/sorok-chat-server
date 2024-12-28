import { ConsoleLogger, Module } from "@nestjs/common";
import { ConfigModule } from "@nestjs/config";

@Module({
  imports: [ConfigModule.forRoot()],
  providers: [ConsoleLogger],
})
export class AppModule {}
