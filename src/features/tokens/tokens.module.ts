import { Module } from "@nestjs/common";
import { TokensService } from "./tokens.service";
import { JwtService } from "@nestjs/jwt";
import { ConfigModule } from "@nestjs/config";

@Module({
  imports: [ConfigModule],
  providers: [TokensService, JwtService],
  exports: [TokensService],
})
export class TokensModule {}
